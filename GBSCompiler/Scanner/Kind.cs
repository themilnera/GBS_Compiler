
namespace GBSCompiler
{
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

		INLINEASM,//  `
		QUT,//  "

		IF,
		ELSE,
		WHILE,
		FOR,

		CALL,
		FUNC,
		RET,

		LPAR,//  (
		RPAR,//  )
		LSQU,//  [
		RSQU,//  ]
		LBRAC,// {
		RBRAC,// }
		SEMC,//  ;
		COM, //  ,
		CMT,//   //

		EOF
	}

}
