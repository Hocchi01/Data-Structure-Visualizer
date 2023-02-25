namespace DataStructureVisualizer.Common.Messages
{
    class LoadAddAnimationMessage
    {
        public int Index { get; }
        public int Value { get; }
        public LoadAddAnimationMessage(int index, int value) 
        {
            Index = index;
            Value = value;
        }
    }
}
