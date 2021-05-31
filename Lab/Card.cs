using System;
namespace Lab
{
	public class Card
    {
        public Card(string s)
        {
            if (string.IsNullOrEmpty(s) || s.Length != 2)
                throw new Exception("Invalid input");
            switch (s[0])
            {
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    Value = s[0] - '0';
                    break;
                case 'T':
                    Value = 10;
                    break;
                case 'J':
                    Value = 11;
                    break;
                case 'Q':
                    Value = 12;
                    break;
                case 'K':
                    Value = 13;
                    break;
                case 'A':
                    Value = 14;
                    break;
                default:
                    break;
            }
            switch (s[1])
            {
                case 'S':
                    Suit = Suit.Spades;
                    break;
                case 'H':
                    Suit = Suit.Hearts;
                    break;
                case 'D':
                    Suit = Suit.Diamonds;
                    break;
                case 'C':
                    Suit = Suit.Clubs;
                    break;
                default:
                    break;
            }
        }

        public Card(int value, Suit suit)
		{
            Value = value;
            Suit = suit;
		}

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Card card = (Card)obj;
                return (Value == card.Value) && (Suit == card.Suit);
            }

        }


        public Suit Suit { get; set; }

        //value - 2-9, T (10) J (11), Q(12), K(13), A(14)
        public int Value { get; set; }
    }

}
