
// Con ngZone:
@Component({
  template: `{{ value }}`
})
export class Demo {
  value = 0;
  ngOnInit() {
    setTimeout(() => this.value++, 1000);
  }
}





// Sin ngZone: 
@Component({
  template: `{{ value() }}`
})
export class Demo {
  value = signal(0);

  ngOnInit() {
    setTimeout(() => this.value.update(v => v + 1), 1000);
  }
}