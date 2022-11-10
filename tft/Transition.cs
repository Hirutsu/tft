namespace tft
{
    class Transition<TStateName, TMover>
    {
        public readonly TStateName StartStateName;
        public readonly TStateName EndStateName;
        public readonly TMover Mover;

        public Transition(TStateName startStateName, TStateName endStateName, TMover mover)
        {
            StartStateName = startStateName;
            EndStateName = endStateName;
            Mover = mover;
        }
    }
}