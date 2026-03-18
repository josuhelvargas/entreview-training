
class Car{

    constructor( public brand:string,
                  public model:number){

    }


}


class CarBuilder {
    private brand!:string;
    private model!:number;
    // private year:number;
    // private color:string;
    // private engine:string

    setBrand(brand:string) {
        this.brand =brand;
        return this;
    }

    setModel(model:number){
        this.model =model;
        return this;
    }

    build():Car{
        return new Car (
            this.brand,
            this.model
        )
    }
}



function Testeando() {
    const mycar :Car = new CarBuilder()
                        .setBrand("Infiniti")
                        .setModel(2017)
                        .build();
}