abstract class Utils {

  constructor() {

  }

  // log(msg: string, level: ErrorLevel): void {
  //   console.log(msg);
  // }

}


class ServiceUtils extends Utils {
  log(msg: string, level: ErrorLevel): void {
    throw new Error("Method not implemented.");
  }

  constructor() {
    super();
  }


  static log(msg: string, level: ErrorLevel): void {
    if (level === ErrorLevel.INFO) {
      console.log(msg);
    } else if (level === ErrorLevel.WARN) {
      console.warn(msg);
    } else {
      console.error(msg);
    }
  }

  static manageDates(date: string): Temporal.PlainDate {
    return Temporal.PlainDate.from(date);
  }

}

enum ErrorLevel {
  INFO = "info",
  WARN = "warn",
  ERROR = "error"
}

