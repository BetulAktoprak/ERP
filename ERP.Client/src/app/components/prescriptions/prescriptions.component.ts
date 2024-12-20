import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { PrescriptionModel } from '../../models/prescription.model';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { NgForm } from '@angular/forms';
import { SharedModule } from '../../modules/shared.module';
import { ProductModel } from '../../models/product.model';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-prescriptions',
  standalone: true,
  imports: [SharedModule, RouterLink],
  templateUrl: './prescriptions.component.html',
  styleUrl: './prescriptions.component.css'
})
export default class PrescriptionsComponent {
  data = signal<PrescriptionModel[]>([]);
  products = signal<ProductModel[]>([]);
  loading = signal<boolean>(false);

  hub: signalR.HubConnection | undefined;

  @ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined;  

  createModel = signal<PrescriptionModel>(new PrescriptionModel());  
  
  #http = inject(HttpService);
  #swal = inject(SwalService);

  ngOnInit(): void {
    this.getAll();
    this.getProducts();
  }

  getAll() {
    this.loading.set(true);
    this.#http.get<PrescriptionModel[]>("Prescriptions/GetAll", (res) => {
      this.data.set(res);
      this.loading.set(false);
    },()=> {
      this.loading.set(false);
    });
  }

  getProducts(){
    this.#http.get<ProductModel[]>("Products/GetAll",(res)=> this.products.set(res));
  }

  create(form: NgForm) {
    if (form.valid) {
      this.#http.post<PrescriptionModel>("Prescriptions/Create", this.createModel(), (res) => {
        this.#swal.callToast("Create is successful");        
        this.data.update((prev)=> [...prev, res]);
        this.createModel.set(new PrescriptionModel());
        this.createModalCloseBtn?.nativeElement.click();
      });
    }
  }

  deleteById(model: PrescriptionModel) {
    this.#swal.callSwal("Reçeteyi Sil?", `${model.productName} reçeteyi silmek istiyor musunuz?`, () => {
      this.#http.get<string>(`Prescriptions/DeleteById?id=${model.id}`, (res) => {        
        this.#swal.callToast(res, "info");
        this.data.update((prev)=> [...prev.filter(p=> p.id != model.id)]);
      });
    })
  }
}
