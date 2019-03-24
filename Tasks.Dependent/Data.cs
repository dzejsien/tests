using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks.Dependent
{
    public class DataA : MockedProcessor, IDependent
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield break;
            }
        }
    }

    public class DataB : MockedProcessor, IDependent
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield break;
            }
        }
    }

    public class DataC : MockedProcessor, IDependent
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield break;
            }
        }
    }

    public class DataD : MockedProcessor, IDependent
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield break;
            }
        }
    }

    public class DataA2 : MockedProcessor, IDependent
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataA);
            }
        }
    }

    public class DataB2 : MockedProcessor, IDependent
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataB);
            }
        }
    }

    public class DataC2 : MockedProcessor, IDependent
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataC);
            }
        }
    }

    public class DataD2 : MockedProcessor, IDependent
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataD);
            }
        }
    }

    public class DataA22 : MockedProcessor, IDependent
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataA);
            }
        }
    }

    public class DataB3 : MockedProcessor, IDependent
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataB2);
            }
        }
    }

    public class DataC22 : MockedProcessor, IDependent
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataC);
            }
        }
    }

    public class DataD3 : MockedProcessor, IDependent
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataD2);
            }
        }
    }

    public class DataAB2 : MockedProcessor, IDependent
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataA);
                yield return typeof(DataB2);
            }
        }
    }
}
