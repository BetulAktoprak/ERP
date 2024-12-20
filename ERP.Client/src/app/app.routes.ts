import { Routes } from '@angular/router';
import { LayoutsComponent } from './components/layouts/layouts.component';
import { HomeComponent } from './components/home/home.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
    {
        path: "login",
        loadComponent: ()=> import("./components/login/login.component")
    },
    {
        path: "confirm-email/:code",
        loadComponent: () => import("./components/confirm-email/confirm-email.component")
    },
    {
        path: "",
        component: LayoutsComponent,
        canActivateChild: [authGuard],
        children: [
            {
                path: "",
                component: HomeComponent
            },
            {
                path: "products",
                children: [
                    {
                        path: "",
                        loadComponent: ()=> import("./components/products/products.component")
                    },
                    {
                        path: "details/:id",
                        loadComponent: ()=> import("./components/stock-movements/stock-movements.component")
                    }
                ]
                
            },
            {
                path: "users",
                loadComponent: ()=> import("./components/users/users.component")
            },
            {
                path: "prescriptions",
                children: [
                    {
                        path: "",
                        loadComponent:()=> import("./components/prescriptions/prescriptions.component")
                    },
                    {
                        path: "details/:id",
                        loadComponent:()=> import("./components/prescription-details/prescription-details.component")
                    }
                ]               
            },
            
        ]
    }
];
