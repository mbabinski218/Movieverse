export class FormDataHelper {
  static toFormData(obj: any): FormData {
    const formData = new FormData();
    this.objectToFormData(obj, '', formData);
    return formData;
  }

  private static objectToFormData(obj: any, parentKey: string, formData: FormData): void {
    if (obj === null || obj === undefined) {
      return;
    }
  
    Object.entries(obj).forEach(([key, value]) => {
      const formKey = parentKey ? `${parentKey}[${key}]` : key;
  
      if (value instanceof File) {
        formData.append(formKey, value);
      } 
      else if (value instanceof FileList) {
        Array.from(value).forEach(file => formData.append(formKey, file));
      } 
      else if (Array.isArray(value)) {
        value.forEach((item, index) => {
          if (typeof item === 'object' && !(item instanceof Date)) {
            this.objectToFormData(item, `${formKey}[${index}]`, formData);
          } else {
            formData.append(`${formKey}[]`, item.toString());
          }
        });
      } 
      else if (typeof value === 'object' && !(value instanceof Date)) {
        this.objectToFormData(value, formKey, formData);
      } 
      else if (value instanceof Date) {
        formData.append(formKey, value.toISOString());
      } 
      else if (value !== null && value !== undefined) {
        formData.append(formKey, value.toString());
      }
    });
  }
}