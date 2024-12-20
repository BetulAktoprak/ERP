import { Component, inject, OnInit, signal, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from '../../services/http.service';
import { FlexiToastService } from 'flexi-toast';

@Component({
  selector: 'app-confirm-email',
  standalone: true,
  imports: [],
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.css',
  encapsulation: ViewEncapsulation.None
})
export default class ConfirmEmailComponent implements OnInit {
  message = signal<string>("");
  loading = signal<boolean>(false);

  #activated = inject(ActivatedRoute);
  #http = inject(HttpService);
  #toast = inject(FlexiToastService);
  #router = inject(Router);

  ngOnInit(): void {
    this.#activated.params.subscribe(res=> {
      this.sendConfirm(res["code"]);
    })
  }

  sendConfirm(code: string){    
    this.loading.set(true);
    this.#http.get<string>(`Auth/ConfirmEmail?code=${code}`,(res)=> {
      this.#toast.showToast("Başarılı",res,"success");
      this.loading.set(false);
      this.#router.navigateByUrl("/login");
    },()=> this.#router.navigateByUrl("/login"))
  }

}
