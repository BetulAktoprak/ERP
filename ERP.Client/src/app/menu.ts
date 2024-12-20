export class MenuModel{
    name: string = "";
    icon: string = "";
    url: string = "";
    isTitle: boolean = false;
    subMenus: MenuModel[] = [];
}

export const Menus: MenuModel[] = [
    {
        name: "Ana Sayfa",
        icon: "fa-solid fa-home",
        url: "/",
        isTitle: false,
        subMenus: []
    },
    {
        name: "Kullanıcılar",
        icon: "fa-solid fa-users",
        url: "/users",
        isTitle: false,
        subMenus: []
    },
    {
        name: "Ürünler",
        icon: "fa-solid fa-boxes-stacked",
        url: "/products",
        isTitle: false,
        subMenus: []
    },
    {
        name: "Reçeteler",
        icon: "fa-solid fa-prescription-bottle-medical",
        url: "/prescriptions",
        isTitle: false,
        subMenus: []
    },
]