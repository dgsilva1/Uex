export interface Contact {
  contactId: number;
  name: string;
  cpf: string;
  phone: string;
  userId: string;
  street: string;
  number: number;
  state: string;
  city: string;
  district: string;
  cep: string;
  complement?: string;
  latitude: number;
  longitude: number;
}

