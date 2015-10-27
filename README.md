Constraint Solver
=============

ConstraintSolver is the final project of the course of Advanced Programming, whose goal was to develop a constraint solver and the corresponding recursive descent parser for domain-specific-language from scratch.

The constraint solver deploys a backtracking algorithm with constraint propagation in order to generate the solutions one at time.

Grammar
----------
The constraints recognized by the parser have to be expressed by using the following grammar:

	Dsl      ::= stats sat notSat
	Stats    ::= var "= {" vals "}" stats | ε
	Vals     ::= val seqVals | ε
	SeqVals  ::= "," val seqVals | ε
	Sat      ::= "{" cons "}" | ε
	NotSat   ::= "!{" cons "}" | ε
	Cons     ::=  pair seqCons | ε
	SeqCons  ::= "," pair seqCons | ε
	Pair     ::= "(" val "," val ")"
	Var      ::= identifier
	Val      ::= string

For example, the possible values for the variables `a`,  `b` and  `c` can be expressed as:

	a = {a1, a2, a3}
	b = {b1, b2, b3}
	c = {c1, c2, c3}

while the constraints that variables should (resp. should not) satisfy are expressed as:

	{(a1, c2), (b3, c3), (a3, b1)}
	!{(a1, b1), (a1, c1), (a2, c2)}