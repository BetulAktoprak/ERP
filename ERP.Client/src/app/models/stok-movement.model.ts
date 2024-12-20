import { EntityModel } from "./entity.model";

export class StokMovementModel extends EntityModel{    
    productId: string = ""; 
    quantity: number = 0;
    type: StockMovementTypeModel = new StockMovementTypeModel();
    typeName: string = "";
    giris: number = 0;
    cikis: number = 0;
    bakiye: number = 0;
    date: string = "";
}

export class StockMovementTypeModel {
    value: number = 0;
    name: string = "";
}
