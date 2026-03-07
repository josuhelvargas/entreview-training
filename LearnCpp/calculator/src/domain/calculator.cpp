#include <string>
#include <sstream>
#include <iostream>

// Tokenizer
// Parser
// Evaluator

class Calculator
{
public:
  std::string[] evaluateOperation(std::string[] expression)
  {
    std::set<char> operators = {"+", "-", "*", "/"};

    std::string left;
    std::string right;
    char operator;
    for (size_t i = 0; i < expression.size(); i++)
    {
      if (operators.count(expression[i]))
      { // contar si la posicion en el string es operator
        operator= expression[i];
        left = expression.substr(0, i);   // 1.position, 2.char nums extracted (if skipped: all after this position)
        right = expression.substr(i + 1); // all chars after this index num.
        break;
      }
    }
    std::string[] tokens = {left, operator, right };
    return tokens;
  }

  struct ResultResponse
  {
    std::string message;
    double result;
  }

  ResultResponse executeOperation(const std::array<std::string, 3> &tokens)
  {
    ResultResponse response{};
    double left = 0.0;
    double right = 0.0;

    try
    {
      left = std::stod(tokens[0]);
      right = std::stod(tokens[2]);
    }
    catch (const std::invalid_argument &)
    {
      response.message = "Error: one of the operands is not a valid number.";
      response.result = -1;
      return response;
    }
    catch (const std::out_of_range &)
    {
      response.message = "Error: one of the operands is out of range.";
      response.result = -1;
      return response;
    }

    if (tokens[1] == "+")
    {
      response.result = left + right;
      response.message = "Success";
    }
    else if (tokens[1] == "-")
    {
      response.result = left - right;
      response.message = "Success";
    }
    else if (tokens[1] == "/")
    {
      if (right == 0.0)
      {
        response.message = "Error: could not divide by zero.";
        response.result = -1;
      }
      else
      {
        response.result = left / right;
        response.message = "Success";
      }
    }
    else if (tokens[1] == "*")
    {
      response.result = left * right;
      response.message = "Success";
    }
    else
    {
      response.message = "Error: unsupported operator.";
      response.result = -1;
    }

    return response;
  }
}