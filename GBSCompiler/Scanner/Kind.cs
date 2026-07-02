using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBSCompiler
{
	//STILL MISSING:
	// && || += -= *= /= ++ -- 
	public enum Kind
	{
		IDENTIFIER,
		T_INT8,
		T_INT16,
		T_CHAR,
		T_STRING,
		T_BOOL,
		T_ARRAY,

		INT8,
		INT16,
		CHAR,
		STRING,
		ARRAY,

		ASSN,//  =
		CMP,//  ==
		NE,// !=
		GT,// >
		LT,// <
		GTE,// >=
		LTE,// <=

		PLUS,// +
		MIN,//  -
		MULT,// *
		DIV,//  /
		MOD,//  %

		INC,// ++
		DEC,// --

		ASADD,// +=
		ASMIN,// -=
		ASMULT,// *=
		ASDIV, // /=

		AND,// and &&
		OR,// or &&

		CAR,//  `
		QUT,//  "

		IF,
		ELSE,
		WHILE,
		FOR,

		FUNC,
		RET,

		LPAR,//  (
		RPAR,//  )
		LSQU,//  [
		RSQU,//  ]
		LBRAC,// {
		RBRAC,// }
		SEMC,//  ;
		CMT,//   //

		EOF
	}

}
