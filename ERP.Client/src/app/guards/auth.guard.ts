import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

export const authGuard: CanActivateFn = (route, state) => {

  if(!localStorage.getItem("access-token")){
    const router = inject(Router);
    router.navigateByUrl("/login");
    return false;
  }

  const token: string = localStorage.getItem("access-token")!;
  const decode:any = jwtDecode(token);
  const now = new Date().getTime() / 1000;
  if(now > decode.exp){
    const router = inject(Router);
    router.navigateByUrl("/login");
    return false;
  }
  
  return true;
};
