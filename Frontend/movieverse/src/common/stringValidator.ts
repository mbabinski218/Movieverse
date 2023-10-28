export class StringValidator {

}

declare global {
  interface String {
    isValid(): boolean;
    isValidEmailFormat(): boolean;
  }
}

String.prototype.isValid = function(): boolean {
  const str = this as string;

  return str !== undefined && str !== null && str.trim() !== '';
};

String.prototype.isValidEmailFormat = function(): boolean {
  const str = this as string;

  return /\S+@\S+\.\S+/.test(str);
};