namespace tree_walk_interpreter
{
  abstract class Expression
  {

    public interface IVisitor {
      void Visit(Binary expression);
      void Visit(Grouping expression);
      void Visit(Literal expression);
      void Visit(Unary expression);
    }


    public abstract void Accept<T>(IVisitor visitor);

    public class Binary: Expression
    {
      public Expression Left {get;}
      public Token OperatorToken {get;}
      public Expression Right {get;}

      Binary (Expression left, Token operatorToken, Expression right)
      {
        this.Left = left;
        this.OperatorToken = operatorToken;
        this.Right = right;
      }
      public override void Accept<T>(IVisitor visitor) {
        visitor.Visit(this);
      }
    }
    public class Grouping: Expression
    {
      public Expression Expression {get;}

      Grouping (Expression expression)
      {
        this.Expression = expression;
      }
      public override void Accept<T>(IVisitor visitor) {
        visitor.Visit(this);
      }
    }
    public class Literal: Expression
    {
      public object Value {get;}

      Literal (object value)
      {
        this.Value = value;
      }
      public override void Accept<T>(IVisitor visitor) {
        visitor.Visit(this);
      }
    }
    public class Unary: Expression
    {
      public Token OperatorToken {get;}
      public Expression Right {get;}

      Unary (Token operatorToken, Expression right)
      {
        this.OperatorToken = operatorToken;
        this.Right = right;
      }
      public override void Accept<T>(IVisitor visitor) {
        visitor.Visit(this);
      }
    }

  }
}
