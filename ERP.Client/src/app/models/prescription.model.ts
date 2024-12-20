import { EntityModel } from "./entity.model";
import { ProductModel } from "./product.model";

export class PrescriptionModel extends EntityModel{
    productId: string = "";
    productName: string = "";
    details: PrescriptionDetailModel[] = [];
}

export class PrescriptionDetailModel extends EntityModel{
    prescriptionId: string = "";
    productId: string = "";
    product: ProductModel = new ProductModel();
    productName: string = "";
    quantity: number = 0;
}
