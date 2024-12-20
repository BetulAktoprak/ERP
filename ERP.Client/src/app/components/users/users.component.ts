import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { UserModel } from '../../models/user.model';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export default class UsersComponent {
  data = signal<UserModel[]>([]);
  loading = signal<boolean>(false);
  createAndUpdateLoading = signal<boolean>(false);

  @ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined;
  @ViewChild("updateModalCloseBtn") updateModalCloseBtn: ElementRef<HTMLButtonElement> | undefined;

  createModel = signal<UserModel>(new UserModel());
  updateModel = signal<UserModel>(new UserModel());

  #http = inject(HttpService);
  #swal = inject(SwalService);

  ngOnInit(): void {
    this.getAll();
  }

  getAll() {
    this.loading.set(true);
    this.#http.get<UserModel[]>("Users/GetAll", (res) => {
      this.data.set(res);
      this.loading.set(false);
    },()=> {
      this.loading.set(false);
    });
  }

  create(form: NgForm) {
    if (form.valid) {
      this.createAndUpdateLoading.set(true);
      this.#http.post<string>("Users/Create", this.createModel(), (res) => {
        this.#swal.callToast(res);
        this.createModel.set(new UserModel());
        this.createModalCloseBtn?.nativeElement.click();        
        this.getAll();
        this.createAndUpdateLoading.set(false);
      },()=> this.createAndUpdateLoading.set(false));
    }
  }

  deleteById(model: UserModel) {
    this.#swal.callSwal("Kullanıcıyı Sil?", `${model.fullName} adlı kullanıcıyı silmek istiyor musunuz?`, () => {
      this.#http.get<string>(`Users/DeleteById?id=${model.id}`, (res) => {        
        this.#swal.callToast(res, "info");
        this.getAll();
      });
    })
  }

  get(model: UserModel) {
    this.updateModel.set({ ...model });    
  }

  update(form: NgForm) {
    if (form.valid) {
      this.createAndUpdateLoading.set(true);
      this.#http.post<string>("Users/Update", this.updateModel(), (res) => {
        this.#swal.callToast(res, "info");
        this.updateModalCloseBtn?.nativeElement.click();        
        this.getAll();
        this.createAndUpdateLoading.set(false);
      },()=> this.createAndUpdateLoading.set(false));
    }
  }
}
