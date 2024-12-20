import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { PrescriptionDetailModel, PrescriptionModel } from '../../models/prescription.model';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { ActivatedRoute } from '@angular/router';
import { ProductModel } from '../../models/product.model';

@Component({
  selector: 'app-prescription-details',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './prescription-details.component.html',
  styleUrl: './prescription-details.component.css'
})
export default class PrescriptionDetailsComponent {
  data = signal<PrescriptionModel>(new PrescriptionModel());  
  products = signal<ProductModel[]>([]);
  createModel = signal<PrescriptionDetailModel>(new PrescriptionDetailModel());  
  loading = signal<boolean>(false);
  
  @ViewChild('productSelect') productSelect: ElementRef | undefined;

  #http = inject(HttpService);
  #swal = inject(SwalService);
  #activate = inject(ActivatedRoute);
  
  ngOnInit(): void {
    this.#activate.params.subscribe((res)=> {
      this.data().id = res["id"];
      this.get();
      this.getAllProducts();
      this.createModel().prescriptionId = this.data().id;
    });

    this.products
  }

  get() {
    this.loading.set(true);
    this.#http.get<PrescriptionModel>(`PrescriptionDetails/Get?prescriptionId=${this.data().id}`, (res) => {
      this.data.set(res);
      this.loading.set(false);
    },()=> {
      this.loading.set(false);
    });
  }  

  getAllProducts() {    
    this.#http.get<ProductModel[]>("Products/GetAll", (res) => {
      this.products.set(res);      
    });
  }

  setProduct(productId:string){    
    const product = this.products().find((x)=> x.id == productId);
    this.createModel().productName = product!.name;
  }

  delete(item:any){
    this.#swal.callSwal("Sil?",`Reçeteden ${item.productName} adlı ürünü silmek istiyor musunuz?`,()=>{
      this.#http.get<string>(`PrescriptionDetails/DeleteById?id=${item.id}`, (res) => {
        this.#swal.callToast(res);
        this.data().details = this.data().details.filter((x)=> x.id != item.id);
      });
    })
  }

  create() {
    if (this.createModel().productId && this.createModel().quantity) {
      this.#http.post<string>("PrescriptionDetails/Create", this.createModel(), (res) => {
        this.#swal.callToast(res);
        this.data().details.push({...this.createModel()});
        this.createModel.set(new PrescriptionDetailModel());
        this.createModel().prescriptionId = this.data().id;
        console.log(this.productSelect);
        
      });
    }
  }
}
