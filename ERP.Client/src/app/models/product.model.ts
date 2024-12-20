import { EntityModel } from "./entity.model";

export class ProductModel extends EntityModel{
    name: string = "";
    type: {name: string, value: number} = {name: "", value: 1}
    typeName: string = "";
    typeValue: number = 1;    
}