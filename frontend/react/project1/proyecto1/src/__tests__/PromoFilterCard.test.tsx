import React from "react";
import { describe, it, expect, beforeEach, afterAll, test } from "vitest";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { PromoFilterCard } from "../components/PromoFIlterCard";

describe("PromoFilterCard", () => {

  // beforeEach(() => {
  // });

  // afterAll(() => {
  // });

  //it y test son equivalentes ( son pra hacarer purebas untiarias indivuduales)
  test("shows promofilterFilterCard component",() => {
    render(<PromoFilterCard participationTypes={[]} status="idle" />);
  });


  //en la siguiebte prueba el erro solo aparece despuesd e un cambio de estadop por tanto esto esta mal  y debe ser como en 2.
  // it("shows error message if code is invalid",()=>{
  //   render(<PromoFilterCard participationTypes={[]} status="success" />); //renderizar 
  //   const input = screen.getByLabelText("access-code");   //localizar elementos en la ui por label/ role 
  //   userEvent.type(input, "invalid-code");
  //   const alert = screen.getByRole("alert");
  //   expect(alert).toBeInTheDocument();
  // });

  //2
  it("shows error message if code is invalid",async ()=>{
    render(<PromoFilterCard participationTypes={[]} status="success" />);
    const user = userEvent.setup();
    const input = screen.getByLabelText("access-code");
    await user.type(input, "invalid-code");
    const alert = await screen.findByRole("alert");
    expect(alert).toBeInTheDocument();

  });

});
