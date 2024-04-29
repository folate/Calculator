using System;
using System.Data;
using System.Reactive;
using ReactiveUI;
namespace calculator.ViewModels;


public class MainWindowViewModel : ViewModelBase
{
    
    public MainWindowViewModel()
    {
        AddCommand = ReactiveCommand.Create<string>(Add);
    }

    public ReactiveCommand<string, Unit> AddCommand { get; }

    private string _memory = "0";
    public string Memory
    {
        get => _memory;
        set => this.RaiseAndSetIfChanged(ref _memory, value);
    }
    private string _expression = "0";
    public string Expression
    {
        get => _expression;
        set => this.RaiseAndSetIfChanged(ref _expression, value);
    }
    private void Add(string content)
    {
        if (int.TryParse(content, out int num))
        {
            AddNumber(num);
        }
        else
        {
            AddOperator(content);
        }
    }
    public void AddNumber(int num)
    {
        if (string.IsNullOrEmpty(Expression) || Expression == "0")
        {
            Expression = num.ToString();
        }
        else
        {
            Expression += num;
        }
    }

    public void MemoryOperator(string op)
    {
        if (op == "MC")
        {
            Memory = "0";
        }
        else if (op == "MR")
        {
            Expression += Memory;
        }
        else if (op == "MS")
        {
            Memory = Expression;
            Expression = "0";
        }
        else if (op == "M+")
        {
            Memory = new DataTable().Compute(Memory + "+" + Expression, null).ToString();
        }
        else if (op == "M-")
        {
            Memory = new DataTable().Compute(Memory + "-" + Expression, null).ToString();
        }
    }
    public void AddOperator(string op)
    {
        if (op == "=")
        {
            if (Expression.Contains(","))
            {
                Expression = new DataTable().Compute(Expression.Replace(",", "."), null).ToString();
                if (Expression.EndsWith(",0"))
                {
                    Expression = Expression.Substring(0, Expression.Length - 2);
                }
            }
            if (Expression.Contains("/0"))
            {
                Expression = "NaN";
            }
            else
            {
                Expression = new DataTable().Compute(Expression, null).ToString();
            }
        }
        else if (op == "/")
        {
            if (Expression == "0")
            {
                Expression = "NaN";
            }
            else
            {
                Expression += op;
            }
        }
        else if (op == "remove")
        {
            if (Expression.Length > 1)
            {
                Expression = Expression.Substring(0, Expression.Length - 1);
            }
            else
            {
                Expression = "0";
            }
        }
        else if (op == "ce")
        {
            Expression = "0";
        }
        else if (op == "c")
        {
            Expression = "0";
        }
        else if (op == "square")
        {
            Expression = new DataTable().Compute(Expression + "*" + Expression, null).ToString();
        }
        else if (op == "sqrt")
        {
            Expression = Math.Sqrt(double.Parse(Expression)).ToString();
        }
        else if (op == "ptm")
        {
            if (Expression.StartsWith("-"))
            {
                Expression = Expression.Substring(1);
            }
            else
            {
                Expression = "-" + Expression;
            }
        }
        else if (op == "percent")
        {
            Expression = new DataTable().Compute(Expression + "/100", null).ToString(); 
        }
        else if (op == "reciproc")
        {
            Expression = new DataTable().Compute("1/" + Expression, null).ToString();
        }
        else
        {
            Expression += op;
        }
    }
}