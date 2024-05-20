export interface Product {
  id?: string;
  Id?: string;
  creationTime?: Date;
  CreationTime?: Date;
  lastUpdate?: Date;
  LastUpdate?: Date;
  deletionTime?: Date;
  DeletionTime?: Date;
  name?: string;
  Name?: string;
  quantity?: number;
  Quantity?: number;
  value?: number;
  Value?: number;
  imagePath?: string;
  ImagePath?: string;
  description?: string;
  Description?: string;
  colors?: string;
  Colors?: string;
  colorsArr?: string[];
  ColorsArr?: string[];
  code?: string;
  Code?: string;
  status?: string;
  Status?: string;
  vendor?: string;
  Vendor?: string;
  label?: string;
  Label?: string;
  category?: string;
  Category?: string;
}
export interface Color {
  Value?: string;
}
