using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NNetTesting.DatasetTools {
    internal class EquationParser {

        List<String> processedEquations;
        public EquationParser(string equation) {
            String processedEquation = process(equation);
            
            processedEquations = parse(processedEquation);
        }

        public double calculate(double f) {
            List<String> replacedOperations = replace(f, processedEquations);
            List<String> compressedEquations = compress(replacedOperations);
            return multiVarCompress(compressedEquations);
        }

        private String process(String equation) {
            equation = equation.Replace(" ", "");
            equation = Regex.Replace(equation, "\\d\\(", "*(");
            return Regex.Replace(equation, "\\)\\d", ")*");

        }
        private List<String> parse(String equation) {
            List<String> substrings = new List<string>();
            while (equation.Contains("(")) {
                int closePar = equation.IndexOf(')');
                int OpenPar = 0;
                for (int i = closePar; !equation.Substring(i, 1).Equals("("); i--) { 
                    //Console.WriteLine(equation.Substring(i, closePar - i));
                    //Console.WriteLine(equation.Substring(i, 1));
                    OpenPar = i; 
                }
                String segment = equation.Substring(OpenPar, closePar - OpenPar);
                substrings.Add(segment);
                equation = equation.Remove(OpenPar, closePar - OpenPar);
                equation = equation.Replace("()", "@" + (substrings.Count-1));
             }
            substrings.Add(equation);
            return substrings;
        }
        private List<String> replace(double x,List<String> equaitons) {
            List<String> replacedEquations = equaitons.ToArray().ToList();
            for (int i = 0; i < replacedEquations.Count(); i++) {
                replacedEquations[i] = replacedEquations[i].Replace("x", x.ToString());
            }
            return replacedEquations;
        }
        private List<String> compress(List<String> segments) {
            String[] mathmaticalOperaitons = { "+", "-", "*", "/", "^"};
            for (int k = 0; k < segments.Count(); k++) {
                
                for (int i = 0; i < segments.Count(); i++) {
                    int operands = 0;
                    foreach (String operand in mathmaticalOperaitons) {
                        if (segments[i].Contains(operand)) {
                            operands++;
                        }
                    }
                    if (operands == 1) {
                        //Console.WriteLine(segments[i]);
                        foreach (String operand in mathmaticalOperaitons) {
                            if (segments[i].Contains(operand) && !segments[i].Contains("@")) {
                                if (operand.Equals("sin")) {
                                    
                                    double n = double.Parse(segments[i].Substring(0, segments[i].IndexOf(operand)+3));
                                    segments[i] = Math.Sin(n).ToString();
                                    continue;
                                }
                                double n1 = double.Parse(segments[i].Substring(0, segments[i].IndexOf(operand)));
                                double n2 = double.Parse(segments[i].Substring(segments[i].IndexOf(operand) + 1));
                                if (operand == "+") {
                                    segments[i] = (n1 + n2).ToString();
                                } else if (operand == "-") {
                                    segments[i] = (n1 - n2).ToString();
                                } else if (operand == "*") {
                                    segments[i] = (n1 * n2).ToString();
                                } else if (operand == "^") {
                                    segments[i] = Math.Pow(n1, n2).ToString();
                                }
                            }
                        }
                    }
                    // Console.WriteLine(segments[i]);
                }
                for (int i = 0; i < segments.Count(); i++) {
                    if (segments[i].Contains('@')) {
                        
                        int refrencedSegment = int.Parse(segments[i].Substring(segments[i].IndexOf("@") + 1, 1));
                        int operands = 0;
                        foreach (String operand in mathmaticalOperaitons) {
                            if (segments[refrencedSegment].Contains(operand)) {
                                operands++;
                            }
                        }
                        if (operands == 0) {
                            
                            segments[i] = segments[i].Replace(segments[i].Substring(segments[i].IndexOf("@"), 2), segments[refrencedSegment]);
                            segments[refrencedSegment] = "";
                        }
                    }
                    if (segments[i].Contains('@')) {
                        
                        int refrencedSegment = int.Parse(segments[i].Substring(segments[i].LastIndexOf("@") + 1, 1));
                        int operands = 0;
                        foreach (String operand in mathmaticalOperaitons) {
                            if (segments[refrencedSegment].Contains(operand)) {
                                operands++;
                            }
                        }
                        if (operands == 0) {
                            
                            segments[i] = segments[i].Replace(segments[i].Substring(segments[i].LastIndexOf("@"), 2), segments[refrencedSegment]);
                            segments[refrencedSegment] = "";
                        }
                    }
                }
            }

            //List<String> compressedEqiations = new List<string>();
            //foreach(String s in segments) {
            //    if (!s.Equals("")) {
            //        compressedEqiations.Add(s);
            //    }
            //}

            return segments;
        }
        public double multiVarCompress(List<String> compressedEquations) {
            double result = 0;
            String[] mathmaticalOperaitons = { "+", "-", "*", "/", "^", "sin" };
            for (int i = 0; i < compressedEquations.Count; i++) {
                String s = compressedEquations[i];
                if (s.Contains("@")) {
                    continue;
                }
                
                List<int> operands = new List<int>();
                foreach (String operand in mathmaticalOperaitons) {
                    if (s.Contains(operand)) {
                        for (int k = 0; k < s.Length; k++) {
                            if (!operands.Contains(s.IndexOf(operand))) {
                                operands.Add(s.IndexOf(operand));
                            }
                        }
                    }
                }
                operands.Sort();
                for (int k = 0; k < operands.Count - 1; k++) {
                    double n1 = double.Parse(s.Substring(0, operands[k]));
                    double n2 = double.Parse(s.Substring(operands[k] + 1, operands[k + 1] - operands[k] - 1));
                    String operand = s.Substring(operands[k], 1);
                    if (operand == "+") {
                        s = s.Replace(s.Substring(0, operands[k + 1] - operands[k] - 1), (n1 + n2).ToString());
                    } else if (operand == "-") {
                        s = s.Replace(s.Substring(0, operands[k + 1] - operands[k] - 1), (n1 - n2).ToString());
                    } else if (operand == "*") {
                        s = s.Replace(s.Substring(0, operands[k + 1] - operands[k] - 1), (n1 * n2).ToString());
                    } else if (operand == "^") {
                        s = s.Replace(s.Substring(0, operands[k + 1] - operands[k] - 1), Math.Pow(n1, n2).ToString());
                    }
                }
                compressedEquations[i] = s;
            }
           for (int i = 0; i < compressedEquations.Count; i++) {
                    int operandios = 0;
                String s = compressedEquations[i];
                foreach (String operand in mathmaticalOperaitons) {
                    if (s.Contains(operand)) {
                        operandios++;
                    }
                }
                if (operandios == 1) {
                    //Console.WriteLine(segments[i]);
                    foreach (String operand in mathmaticalOperaitons) {
                        if (s.Contains(operand) && !s.Contains("@")) {
                            double n1 = double.Parse(s.Substring(0, s.IndexOf(operand)));
                            double n2 = double.Parse(s.Substring(s.IndexOf(operand) + 1));
                            if (operand == "+") {
                                s = (n1 + n2).ToString();
                            } else if (operand == "-") {
                                s = (n1 - n2).ToString();
                            } else if (operand == "*") {
                                s = (n1 * n2).ToString();
                            } else if (operand == "^") {
                                s = Math.Pow(n1, n2).ToString();
                            }
                        }
                    }
                    // Console.WriteLine(segments[i]);
                }
                if (s.Contains('@')) {

                    int refrencedSegment = int.Parse(s.Substring(s.IndexOf("@") + 1, 1));
                    int operandioos = 0;
                    foreach (String operand in mathmaticalOperaitons) {
                        if (compressedEquations[refrencedSegment].Contains(operand)) {
                            operandioos++;
                        }
                    }
                    if (operandioos == 0) {

                        s = s.Replace(s.Substring(s.IndexOf("@"), 2), compressedEquations[refrencedSegment]);
                        compressedEquations[refrencedSegment] = "";
                    }
                }
                if (s.Contains('@')) {

                    int refrencedSegment = int.Parse(s.Substring(s.LastIndexOf("@") + 1, 1));
                    int operandioos = 0;
                    foreach (String operand in mathmaticalOperaitons) {
                        if (compressedEquations[refrencedSegment].Contains(operand)) {
                            operandioos++;
                        }
                    }
                    if (operandioos == 0) {

                        s = s.Replace(s.Substring(s.LastIndexOf("@"), 2), compressedEquations[refrencedSegment]);
                        compressedEquations[refrencedSegment] = "";
                    }
                }
                
                Console.WriteLine(s);
                compressedEquations[i] = s;
            }
            return result;
        }

    }
}
