export class FormDataHelper {
  static toFormData(obj: any): FormData {
    const formData = new FormData();

    Object.entries(obj as any).forEach(([key, value]) => {
      if (value instanceof Object) {
        this.childToFormData(value, key, formData);
      } 
      else if (value) {
        formData.append(key, value as string);
      }
    });
  
    return formData;
  }

  static childToFormData(obj: Object, parentName: string, formData: FormData): void {
    Object.entries(obj).forEach(([key, value]) => {
      if (value instanceof Object) {
        this.childToFormData(value, `${parentName}.${key}`, formData);
      } 
      else if (value) {
        formData.append(`${parentName}.${key}`, value as string);
      }
    });
  }
}