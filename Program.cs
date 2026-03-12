namespace Calculator;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        PrintHeader();

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Введите первое число (или 'q' для выхода):");
            string? firstInput = Console.ReadLine()?.Trim();

            if (IsExitCommand(firstInput))
                break;

            if (!TryParseNumber(firstInput, out double firstNumber))
            {
                PrintError("Некорректное число, введите повторно");
                continue;
            }

            Console.WriteLine("Введите второе число:");
            string? secondInput = Console.ReadLine()?.Trim();

            if (IsExitCommand(secondInput))
                break;

            if (!TryParseNumber(secondInput, out double secondNumber))
            {
                PrintError("Некорректное число, введите повторно");
                continue;
            }

            Console.WriteLine("Выберите операцию:");
            Console.WriteLine("  +  Сложение");
            Console.WriteLine("  -  Вычитание");
            Console.WriteLine("  *  Умножение");
            Console.WriteLine("  /  Деление");
            Console.Write("> ");
            string? operation = Console.ReadLine()?.Trim();

            if (IsExitCommand(operation))
                break;

            double? result = Calculate(firstNumber, secondNumber, operation);

            if (result is null)
            {
                PrintError("Ошибка вычисления или не поддерживаемая операция");
                continue;
            }

            string operationSymbol = operation ?? "?";
            Console.WriteLine(new string('─', 30));
            Console.WriteLine($"  {FormatNumber(firstNumber)} {operationSymbol} {FormatNumber(secondNumber)} = {FormatNumber(result.Value)}");
            Console.WriteLine(new string('─', 30));
        }

        Console.WriteLine();
        Console.WriteLine("Запуск завершён");
    }

    static double? Calculate(double a, double b, string? op)
    {
        return op switch
        {
            "+" => a + b,
            "-" => a - b,
            "*" => a * b,
            "/" when b == 0 => PrintDivisionByZeroError(),
            "/" => a / b,
            _ => null
        };
    }

    static double? PrintDivisionByZeroError()
    {
        PrintError("Деление на ноль, ошибка");
        return null;
    }

    static bool TryParseNumber(string? input, out double number)
    {
        return double.TryParse(
            input?.Replace(',', '.'),
            System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture,
            out number);
    }

    static bool IsExitCommand(string? input) =>
        string.Equals(input, "q", StringComparison.OrdinalIgnoreCase);

    static string FormatNumber(double number)
    {
        return number == Math.Floor(number) && !double.IsInfinity(number)
            ? number.ToString("0")
            : number.ToString("G10");
    }

    static void PrintHeader()
    {
        Console.WriteLine("Это приложение-калькулятор");
        Console.WriteLine("Операции: +, - ,* ,/");
        Console.WriteLine("Выход: q");
    }

    static void PrintError(string message)
    {
        ConsoleColor prev = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: {message}");
        Console.ForegroundColor = prev;
    }
}
