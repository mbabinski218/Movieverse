export type Information = {
  FirstName: string | null;
  LastName: string | null;
  Age: number;
}

export type LifeHistory = {
  BirthPlace: string | null;
  BirthDate: string | null;
  DeathPlace: string | null;
  DeathDate: string | null;
  CareerStart: string | null;
  CareerEnd: string | null;
}

export type UpdatePersonContract = {
  Information: Information;
  LifeHistory: LifeHistory;
  Biography: string | null;
  FunFacts: string | null;
  Picture: File | null;
  ChangePicture: boolean | null;
  Pictures: File[];
  PicturesToRemove: string[];
}

export class UpdatePersonContractExtensions {
  static initialValues: UpdatePersonContract = {
    Information: {
      FirstName: null,
      LastName: null,
      Age: 0
    },
    LifeHistory: {
      BirthPlace: null,
      BirthDate: null,
      DeathPlace: null,
      DeathDate: null,
      CareerStart: null,
      CareerEnd: null
    },
    Biography: null,
    FunFacts: null,
    Picture: null,
    ChangePicture: null,
    Pictures: [],
    PicturesToRemove: []
  }
}