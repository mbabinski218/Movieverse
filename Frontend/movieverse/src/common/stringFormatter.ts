export class StringFormatter {
  static convertFromCamelCase(inputString: string): string {
    let formattedString = inputString.replace(/([a-z])([A-Z])/g, '$1 $2'); // insert a space between lower & upper
    formattedString = formattedString.toLowerCase(); // convert to lowercase
    formattedString = formattedString.charAt(0).toUpperCase() + formattedString.slice(1) // capitalize first letter

    return formattedString;
  }
}