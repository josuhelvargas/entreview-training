

// export const isNumber = (value) => {
//   return !isNaN(value);
// }

export function processMessage(message: string, logger: (msg: string) => void) {
  logger(message);
}