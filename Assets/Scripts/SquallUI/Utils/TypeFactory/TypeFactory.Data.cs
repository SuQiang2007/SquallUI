using System.Collections.Generic;

public partial class TypeFactory
{
    static TypeFactory()
    {
        RegisterCreator<TestView1>();
        RegisterCreator<TestView2>();
        RegisterCreator<TestView3>();
    }
}
