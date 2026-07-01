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
		T_NUM8,
		T_NUM16,
		T_CHAR,
		T_STRING,
		T_ARRAY,

		NUM8,
		NUM16,
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
