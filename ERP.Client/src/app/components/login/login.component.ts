import { Component, ElementRef, signal, ViewChild } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { LoginModel } from '../../models/login.model';
import { HttpService } from '../../services/http.service';
import { LoginResponseModel } from '../../models/login.response.model';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { FlexiToastService } from 'flexi-toast';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export default class LoginComponent {
  model = signal<LoginModel>(new LoginModel());
  isLoading = signal<boolean>(false);
  email = signal<string>("");
  isSendConfirmEmailLoading = signal<boolean>(false);

  @ViewChild("sendConfirmEmailModalCloseBtn") sendConfirmEmailModalCloseBtn: ElementRef<HTMLButtonElement> | undefined;

  constructor(
    private http: HttpService,
    private router: Router,
    private toast: FlexiToastService
  ){}

  signIn(){
    this.isLoading.set(true);
    this.http.post<LoginResponseModel>("Auth/Login",this.model(),(res)=> {
      localStorage.setItem("access-token", res.accessToken);
      this.router.navigateByUrl("/");      
    },()=> this.isLoading.set(false));
  }

  sendConfirmEmail(form: NgForm){
    if(form.valid){
      this.isSendConfirmEmailLoading.set(true);
      this.http.get<string>(`Auth/SendConfirmEmail?email=${this.email()}`,(res)=> {
        this.toast.showToast("Başarılı",res);
        this.isSendConfirmEmailLoading.set(false);
        this.sendConfirmEmailModalCloseBtn!.nativeElement.click();
      },()=> this.isSendConfirmEmailLoading.set(false));
    }
  }
}
