import { ChangeDetectionStrategy, Component, inject, signal, ViewEncapsulation } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { StokMovementModel } from '../../models/stok-movement.model';
import { HttpService } from '../../services/http.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-stock-movements',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './stock-movements.component.html',
  styleUrl: './stock-movements.component.css',
  encapsulation: ViewEncapsulation.None, 
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class StockMovementsComponent {
 data = signal<StokMovementModel[]>([]);
 productId = signal<string>("");
 loading = signal<boolean>(false);

 #http = inject(HttpService); 
 #activated = inject(ActivatedRoute);

 constructor() {
  this.#activated.params.subscribe((res)=> {
    this.productId.set(res["id"]);
    this.getAll();
  });  
 }

 getAll(){
  this.loading.set(true);
  this.#http.get<StokMovementModel[]>(`StockMovements/GetAll?productId=${this.productId()}`, (res)=> {
    this.data.set(res);
    this.loading.set(false);
  },()=> {
    this.loading.set(false);
  });
 }
}
