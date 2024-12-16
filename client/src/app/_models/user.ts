export interface User {
  login: string;
  token: string;
  contacts: Contact[];
  firstName?: string;
}

export interface Contact {
  contactId: number;
  name: string;
  cpf: string;
  phone: string;
  street: string;
  state: string;
  city: string;
  district: string;
  cep: string;
}
