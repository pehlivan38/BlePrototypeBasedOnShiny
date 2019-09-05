using Shiny;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLEPrototype.Models
{
    public class Group<T> : ObservableList<T>
    {
        public Group(string name, string shortName)
        {
            this.Name = name;
            this.ShortName = shortName;
        }


        public string Name { get; }
        public string ShortName { get; }
    }
}
