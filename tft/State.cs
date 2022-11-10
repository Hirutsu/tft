namespace tft
{
    class State<TStateName/*, TMover*/>
    {
        public readonly TStateName Name;
        //private List<Transition<TStateName, TMover>> Transitions;

        public State(TStateName name)
        {
            Name = name;
        }

        public void StateDo()
        {
            Console.Write(Name.ToString() + ' ');
        }
    }
}