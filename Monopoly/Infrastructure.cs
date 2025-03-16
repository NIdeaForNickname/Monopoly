namespace Monopoly;

internal class Infrastructure : Tile, IGroupable
    {
        
        // temp
        private int Rent = 100;
        public InfrastructureGroup Group { get; set; }
        public int Price { get; private set; }
        public Player Owner { get; private set; }

        public Infrastructure(string name, uint pos, int price) : base(name, pos)
        {
            Price = price;
        }

        public Infrastructure(string name, uint pos, int price, InfrastructureGroup gr) : this(name, pos, price)
        {
            Group = gr;
        }

        public override bool Action(Player player)
        {
            if (player == Owner)
            {
                // later upgrades
                return true;
            }
            else if (Owner != null)
            {
                Transaction tr = new Transaction(
                    Owner,
                    player, 
                    new Transaction.ItemList{},
                    new Transaction.ItemList{ Money = Group.GetPrice(this) }
                    );
                
                return tr.Execute();
            }
            else
            {
                Transaction tr = new Transaction(
                    null,
                    player,
                    new Transaction.ItemList { Properties = new List<IBuyable>(){this} },
                    new Transaction.ItemList { Money = Price }
                );
                return tr.Execute();
            }
            return false;
        }

        public override TurnResult OnStepped(Player player)
        {
            if (Owner == null)
            {
                return new TurnResult {
                    Message = $"{TileName} is available for {Price}.",
                    CanBuy = true,
                    City = this.TileName,
                    MandatoryAction = false
                };
            }
            else if (Owner != player)
            {
                return new TurnResult {
                    Message = $"{player.Name} paid {Rent} to {Owner.Name} for landing on {TileName}.",
                    MoneyNeeded = Rent,
                    Owner = Owner.Name,
                    MandatoryAction = true
                };
            }
            else
            {
                return new TurnResult {
                    Message = $"{player.Name} landed on their property, {TileName}.",
                    MoneyNeeded = Rent,
                    Owner = Owner.Name,
                    MandatoryAction = true
                };
            }
        }

        public bool MoveProperty(Player Own, Player Acc)
        {
            if (Owner != Own)
            {
                return false;
            }
            
            Own?.RemoveProperty(this);
            Acc?.AddProperty(this);
            Owner = Acc;
            return true;
        }

        public int GetPrice()
        {
            return Rent;
        }

        public string GetKey()
        {
            return Owner.Name;
        }
    }