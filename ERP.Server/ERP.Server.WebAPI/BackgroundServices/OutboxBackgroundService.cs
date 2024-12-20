using ERP.Server.Application.Services;
using ERP.Server.Domain.Contants;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Utilities;
using ERP.Server.Infrastructure.Context;
using ERP.Server.WebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace ERP.Server.WebAPI.BackgroundServices;

public sealed class OutboxBackgroundService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            ApplicationDbContext context = ServiceTool.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            MongoClient client = new MongoClient("mongodb+srv://admin:1@erpdb.0nlbn.mongodb.net/");
            IMongoDatabase database = client.GetDatabase("ERPDb");

            IHubContext<DbHub> hubContext = ServiceTool.ServiceProvider.GetRequiredService<IHubContext<DbHub>>();
            IMailService mailService = ServiceTool.ServiceProvider.GetRequiredService<IMailService>();

            #region Create First User
            IMongoCollection<User> _userCollection = database.GetCollection<User>("users");
            FilterDefinition<User> userFilter = Builders<User>.Filter.Eq("IsDeleted", false);
            bool haveUser = await _userCollection.Find(userFilter).AnyAsync(stoppingToken);
            if (!haveUser)
            {
                HashingHelper.CreatePassword("1", out byte[] passwordSalt, out byte[] passwordHash);

                User user = new()
                {
                    FirstName = "Taner",
                    LastName = "Saydam",
                    Email = "tanersaydam@gmail.com",
                    IsEmailConfirmed = true,
                    IsActive = true,
                    UserName = "taner",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                };

                _userCollection.InsertOne(user);
            }
            #endregion

            #region Match Db
            List<MatchDbOutbox> matchDbOutboxes =
                await context.MatchDbOutboxes
                .Where(p => p.IsCompleted == false && p.TryCount < 3)
                .OrderBy(p => p.CreateAt)
                .ToListAsync(stoppingToken);

            foreach (MatchDbOutbox item in matchDbOutboxes)
            {
                if (item.OperationName == OperationNames.Create)
                {
                    if (item.TableName == TableNames.Product)
                    {
                        Product? product = await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);
                        if (product is not null)
                        {
                            try
                            {
                                IMongoCollection<Product> _collection = database.GetCollection<Product>("products");
                                _collection.InsertOne(product);

                                item.IsCompleted = true;
                                await context.SaveChangesAsync(stoppingToken);

                                await hubContext.Clients.All.SendAsync("create-products", product);
                            }
                            catch (Exception)
                            {
                                if (item.TryCount >= 3)
                                {
                                    await context.SaveChangesAsync(stoppingToken);
                                    continue;
                                }

                                item.TryCount++;
                            }
                        }
                    }
                    else if (item.TableName == TableNames.User)
                    {
                        User? user = await context.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);

                        if (user is not null)
                        {
                            try
                            {
                                IMongoCollection<User> _collection = database.GetCollection<User>("users");
                                _collection.InsertOne(user);

                                item.IsCompleted = true;
                                await context.SaveChangesAsync(stoppingToken);
                            }
                            catch (Exception)
                            {
                                if (item.TryCount >= 3)
                                {
                                    await context.SaveChangesAsync(stoppingToken);
                                    continue;
                                }

                                item.TryCount++;
                            }
                        }
                    }
                    else if (item.TableName == TableNames.Prescription)
                    {
                        Prescription? prescription = await context.Prescriptions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);

                        if (prescription is not null)
                        {
                            try
                            {
                                IMongoCollection<Prescription> _collection = database.GetCollection<Prescription>("prescriptions");
                                _collection.InsertOne(prescription);

                                item.IsCompleted = true;
                                await context.SaveChangesAsync(stoppingToken);
                            }
                            catch (Exception)
                            {
                                if (item.TryCount >= 3)
                                {
                                    await context.SaveChangesAsync(stoppingToken);
                                    continue;
                                }

                                item.TryCount++;
                            }
                        }
                    }
                    else if (item.TableName == TableNames.PrescriptionDetail)
                    {
                        PrescriptionDetail? prescriptionDetail = await context.PrescriptionDetails.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);

                        if (prescriptionDetail is not null)
                        {
                            try
                            {
                                IMongoCollection<PrescriptionDetail> _collection = database.GetCollection<PrescriptionDetail>("prescription-details");
                                _collection.InsertOne(prescriptionDetail);

                                item.IsCompleted = true;
                                await context.SaveChangesAsync(stoppingToken);
                            }
                            catch (Exception)
                            {
                                if (item.TryCount >= 3)
                                {
                                    await context.SaveChangesAsync(stoppingToken);
                                    continue;
                                }

                                item.TryCount++;
                            }
                        }
                    }
                }
                else if (item.OperationName == OperationNames.Update)
                {
                    if (item.TableName == TableNames.Product)
                    {
                        Product? product = await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);
                        if (product is not null)
                        {
                            IMongoCollection<Product> _collection = database.GetCollection<Product>("products");
                            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq("_id", item.RecordId);
                            UpdateDefinition<Product> update = Builders<Product>.Update
                                  .Set(p => p.Name, product.Name)
                                  .Set(p => p.Type, product.Type)
                                  .Set(p => p.UpdateAt, product.UpdateAt)
                                  ;

                            UpdateResult updateResult = _collection.UpdateOne(filter, update);
                            if (updateResult.ModifiedCount <= 0)
                            {
                                item.TryCount++;
                            }
                            else
                            {
                                item.IsCompleted = true;
                            }

                            await context.SaveChangesAsync(stoppingToken);
                        }
                    }
                    else if (item.TableName == TableNames.User)
                    {
                        User? user = await context.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);
                        if (user is not null)
                        {
                            IMongoCollection<User> _collection = database.GetCollection<User>("users");
                            FilterDefinition<User> filter = Builders<User>.Filter.Eq("_id", item.RecordId);
                            UpdateDefinition<User> update = Builders<User>.Update
                                  .Set(p => p.FirstName, user.FirstName)
                                  .Set(p => p.LastName, user.LastName)
                                  .Set(p => p.UserName, user.UserName)
                                  .Set(p => p.Email, user.Email)
                                  .Set(p => p.PasswordSalt, user.PasswordSalt)
                                  .Set(p => p.PasswordHash, user.PasswordHash)
                                  .Set(p => p.UpdateAt, user.UpdateAt)
                                  .Set(p => p.MailConfirmCode, user.MailConfirmCode)
                                  .Set(p => p.IsEmailConfirmed, user.IsEmailConfirmed)
                                  .Set(p => p.IsActive, user.IsActive)
                                  ;

                            UpdateResult updateResult = _collection.UpdateOne(filter, update);
                            if (updateResult.ModifiedCount <= 0)
                            {
                                item.TryCount++;
                            }
                            else
                            {
                                item.IsCompleted = true;
                            }

                            await context.SaveChangesAsync(stoppingToken);

                        }
                    }

                }
                else if (item.OperationName == OperationNames.Delete)
                {
                    if (item.TableName == TableNames.Product)
                    {
                        Product? product = await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);
                        if (product is not null)
                        {
                            IMongoCollection<Product> _collection = database.GetCollection<Product>("products");
                            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq("_id", item.RecordId);
                            UpdateDefinition<Product> update = Builders<Product>.Update
                                  .Set(p => p.UpdateAt, product.UpdateAt)
                                  .Set(p => p.IsDeleted, product.IsDeleted)
                                  .Set(p => p.DeleteAt, product.DeleteAt)
                                  ;

                            UpdateResult updateResult = _collection.UpdateOne(filter, update);
                            if (updateResult.ModifiedCount <= 0)
                            {
                                item.TryCount++;
                            }
                            else
                            {
                                item.IsCompleted = true;
                            }

                            await context.SaveChangesAsync(stoppingToken);
                        }
                    }
                    else if (item.TableName == TableNames.User)
                    {
                        User? user = await context.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);
                        if (user is not null)
                        {
                            IMongoCollection<User> _collection = database.GetCollection<User>("users");
                            FilterDefinition<User> filter = Builders<User>.Filter.Eq("_id", item.RecordId);
                            UpdateDefinition<User> update = Builders<User>.Update
                                  .Set(p => p.UpdateAt, user.UpdateAt)
                                  .Set(p => p.IsDeleted, user.IsDeleted)
                                  .Set(p => p.DeleteAt, user.DeleteAt)
                                  ;

                            UpdateResult updateResult = _collection.UpdateOne(filter, update);
                            if (updateResult.ModifiedCount <= 0)
                            {
                                item.TryCount++;
                            }
                            else
                            {
                                item.IsCompleted = true;
                            }

                            await context.SaveChangesAsync(stoppingToken);

                        }
                    }
                    else if (item.TableName == TableNames.Prescription)
                    {
                        Prescription? prescription = await context.Prescriptions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);
                        if (prescription is not null)
                        {
                            IMongoCollection<Prescription> _collection = database.GetCollection<Prescription>("prescriptions");
                            FilterDefinition<Prescription> filter = Builders<Prescription>.Filter.Eq("_id", item.RecordId);
                            UpdateDefinition<Prescription> update = Builders<Prescription>.Update
                                  .Set(p => p.UpdateAt, prescription.UpdateAt)
                                  .Set(p => p.IsDeleted, prescription.IsDeleted)
                                  .Set(p => p.DeleteAt, prescription.DeleteAt)
                                  ;

                            UpdateResult updateResult = _collection.UpdateOne(filter, update);
                            if (updateResult.ModifiedCount <= 0)
                            {
                                item.TryCount++;
                            }
                            else
                            {
                                item.IsCompleted = true;
                            }

                            await context.SaveChangesAsync(stoppingToken);

                        }
                    }
                    else if (item.TableName == TableNames.PrescriptionDetail)
                    {
                        PrescriptionDetail? prescriptionDetail = await context.PrescriptionDetails.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);
                        if (prescriptionDetail is not null)
                        {
                            IMongoCollection<PrescriptionDetail> _collection = database.GetCollection<PrescriptionDetail>("prescription-details");
                            FilterDefinition<PrescriptionDetail> filter = Builders<PrescriptionDetail>.Filter.Eq("_id", item.RecordId);
                            UpdateDefinition<PrescriptionDetail> update = Builders<PrescriptionDetail>.Update
                                  .Set(p => p.UpdateAt, prescriptionDetail.UpdateAt)
                                  .Set(p => p.IsDeleted, prescriptionDetail.IsDeleted)
                                  .Set(p => p.DeleteAt, prescriptionDetail.DeleteAt)
                                  ;

                            UpdateResult updateResult = _collection.UpdateOne(filter, update);
                            if (updateResult.ModifiedCount <= 0)
                            {
                                item.TryCount++;
                            }
                            else
                            {
                                item.IsCompleted = true;
                            }

                            await context.SaveChangesAsync(stoppingToken);

                        }
                    }
                }

                await Task.Delay(300);
            }
            await Task.Delay(10000);
            #endregion

            #region Send Confirm Email
            List<SendConfirmEmailOutBox> sendConfirmEmailOutboxes =
               await context.SendConfirmEmailOutBoxes
               .Where(p => p.IsCompleted == false && p.TryCount < 3)
               .OrderBy(p => p.CreateAt)
               .ToListAsync(stoppingToken);

            foreach (var item in sendConfirmEmailOutboxes)
            {
                var response = await mailService.SendEmailAsync(item.To, item.Subject, item.Body);
                if (response.Successful)
                {
                    item.IsCompleted = true;

                    User? user = await context.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Email == item.To, stoppingToken);
                    if (user is not null)
                    {
                        IMongoCollection<User> _collection = database.GetCollection<User>("users");
                        FilterDefinition<User> filter = Builders<User>.Filter.Eq("_id", user.Id);
                        UpdateDefinition<User> update = Builders<User>.Update
                              .Set(p => p.IsEmailConfirmed, true)
                              .Set(p => p.UpdateAt, user.UpdateAt)
                              ;

                        UpdateResult updateResult = _collection.UpdateOne(filter, update);
                        if (updateResult.ModifiedCount <= 0)
                        {
                            item.TryCount++;
                        }
                        else
                        {
                            item.IsCompleted = true;
                        }

                        await context.SaveChangesAsync(stoppingToken);
                    }
                }
                else
                {
                    item.TryCount++;
                }
                await context.SaveChangesAsync(stoppingToken);
            }
            #endregion
        }
    }
}