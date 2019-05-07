export class User {
    constructor(
        public UserId: string,
        public Name: string,
        public Amount: number,
        public Password?: string) 
        { }
}