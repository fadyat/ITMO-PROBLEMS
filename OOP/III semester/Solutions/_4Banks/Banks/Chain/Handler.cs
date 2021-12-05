namespace Banks.Banks.Chain
{
    public class Handler
    {
        public Handler(Handler successor = null)
        {
            Successor = successor;
        }

        private Handler Successor { get; set; }

        public virtual bool HandlerRequest()
        {
            return Successor == null ||
                   Successor.HandlerRequest();
        }

        public void SetHandler(Handler successor)
        {
            Successor = successor;
        }
    }
}