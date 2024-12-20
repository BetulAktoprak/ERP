import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { ProductModel } from '../../models/product.model';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { NgForm } from '@angular/forms';
import * as signalR from '@microsoft/signalr';
import { mainUrl } from '../../constants';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [SharedModule, RouterLink],
  templateUrl: './products.component.html',
  styleUrl: './products.component.css'
})
export default class ProductsComponent {
  data = signal<ProductModel[]>([]);
  loading = signal<boolean>(false);

  hub: signalR.HubConnection | undefined;

  @ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined;
  @ViewChild("updateModalCloseBtn") updateModalCloseBtn: ElementRef<HTMLButtonElement> | undefined;

  createModel = signal<ProductModel>(new ProductModel());
  updateModel = signal<ProductModel>(new ProductModel());

  #http = inject(HttpService);
  #swal = inject(SwalService);

  ngOnInit(): void {
    this.getAll();

    this.hub = new signalR.HubConnectionBuilder().withUrl(`${mainUrl}/db-hub`).build();

    this.hub.start().then(()=> {
      console.log("Connection is started...");  
      
      this.hub?.on("create-products", (res:ProductModel) => {
        console.log(res);        
      });
    })
  }

  getAll() {
    this.loading.set(true);
    this.#http.get<ProductModel[]>("Products/GetAll", (res) => {
      this.data.set(res);
      this.loading.set(false);
    },()=> {
      this.loading.set(false);
    });
  }

  create(form: NgForm) {
    if (form.valid) {
      this.#http.post<string>("Products/Create", this.createModel(), (res) => {
        this.#swal.callToast(res);
        this.createModel.set(new ProductModel());
        this.createModalCloseBtn?.nativeElement.click();        
        this.getAll();
      });
    }
  }

  deleteById(model: ProductModel) {
    this.#swal.callSwal("Veriyi Sil?", `${model.name} ürününü silmek istiyor musunuz?`, () => {
      this.#http.get<string>(`Products/DeleteById?id=${model.id}`, (res) => {        
        this.#swal.callToast(res, "info");
        this.getAll();
      });
    })
  }

  get(model: ProductModel) {
    console.log(model);
    
    this.updateModel.set({ ...model });
    this.updateModel().typeValue = this.updateModel().typeName == "Mamül" ? 2 : 1;
  }

  update(form: NgForm) {
    if (form.valid) {
      this.#http.post<string>("Products/Update", this.updateModel(), (res) => {
        this.#swal.callToast(res, "info");
        this.updateModalCloseBtn?.nativeElement.click();        
        this.getAll();
      });
    }
  }
}
