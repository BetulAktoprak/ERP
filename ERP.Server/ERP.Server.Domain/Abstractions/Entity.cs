﻿using MongoDB.Bson.Serialization.Attributes;

namespace ERP.Server.Domain.Abstractions;
public abstract class Entity
{
    public Entity()
    {
        Id = Guid.NewGuid();
    }
    [BsonId]
    public Guid Id { get; set; }
    public Guid CreatedUserId { get; set; }
    public DateTime CreateAt { get; set; }
    public Guid? UpdatedUserId { get; set; }
    public DateTime? UpdateAt { get; set; }
    public bool IsDeleted { get; set; }
    public Guid? DeletedUserId { get; set; }
    public DateTime? DeleteAt { get; set; }
}
