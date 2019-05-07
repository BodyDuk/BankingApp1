export class TransactionResult {
    constructor(
        public TimeStemp: string,
        public OperationName: string,
        public Amount: number,
        public SenderName: string,
        public RecipientName: string) 
        { }
}