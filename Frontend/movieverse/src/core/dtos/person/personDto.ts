import { InformationDto } from "../user/userDto"

export type PersonDto = {
  information: InformationDto;
  lifeHistory: LifeHistoryDto;
  biography?: string;
  funFacts?: string;
  pictureId?: string;
  contentsIds: string[];
}

export type LifeHistoryDto = {
  birthPlace?: string;
  birthDate?: string;
  deathPlace?: string;
  deathDate?: string;
  careerStart?: string;
  careerEnd?: string;
}