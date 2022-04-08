grammar Server;

fragment LETTER : [A-Za-z];  
fragment WORD : ((LETTER | '_')+ | '*');
fragment DIGITS : [0-9];
fragment NUMBER : DIGITS+ ('.,' DIGITS+)?;
WS : [ ;\n\t] -> skip;

PACKAGE : 'package';
IMPORT : 'import';
SOURCE : (WORD '.'?)+;

ANNOTATION_HEADER : '@' WORD;
ANNOTATION_ARGS : '(' (KEY_VALUE ','? ' '?)* ')';

KEY : (LETTER | '_')+;
VALUE : (LETTER | '_' | '/' | '"' | '{' | '}' | DIGITS)+;
KEY_VALUE : (VALUE | KEY '=' VALUE);

import_ : IMPORT SOURCE;
package_ : PACKAGE SOURCE;
annotation_ : ANNOTATION_HEADER ANNOTATION_ARGS?;

root : (import_
     |  package_
     |  annotation_
     )+ EOF;
