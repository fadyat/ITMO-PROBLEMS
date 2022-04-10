grammar Server;

PACKAGE                     :       'package';
IMPORT                      :       'import';
MODEL                       :       'class' | 'interface';
ACCESS_MODIFIER             :       'public' | 'private' | 'protected';
NON_ACCESS_MODIFIER         :       'static' | 'abstract' | 'final';
INHERITANCE_TYPE            :       'extends' | 'implements';

WS                  :       [ ;\n\t] -> skip;

fragment LETTER     :       [A-Za-z];  
fragment WORD       :       LETTER+;
fragment DIGIT      :       [0-9];
fragment NUMBER     :       DIGIT+ ('.,' DIGIT+)?;

VARIABLE_NAME       :       (LETTER | '_') (LETTER | DIGIT | '_')*;
VARIABLE_TYPE       :       (LETTER | '<' | '>' | DIGIT | '_')+;
INCLUDE_PATH        :       ((WORD '.'?)* '*') | ((WORD '.'?)+ '*'?);

KEY_VALUE           :       VARIABLE_NAME ' '? '=' ' '? VALUE;
fragment VALUE      :       (STRING | VARIABLE_TYPE | ANY ' '?)+;
ANY                 :       ~([\n;])+?;
STRING              :       '"' ANY+ '"';

package_name        :       PACKAGE;
path                :       (INCLUDE_PATH | VARIABLE_NAME);
import_name         :       IMPORT;
package             :       package_name path;
import_             :       import_name path;

ANNOTATION_HEADER   :       '@' VARIABLE_NAME;
annotation_header   :       ANNOTATION_HEADER;
arguments           :       ANNOTATION_ARGS;
annotation          :       annotation_header arguments?;
fragment ANNOTATION_ARG      :       (KEY_VALUE | VALUE);
ANNOTATION_ARGS     :       '(' (ANNOTATION_ARG ','? ' '?)* ')';

modifier            :       ((ACCESS_MODIFIER NON_ACCESS_MODIFIER?) | (NON_ACCESS_MODIFIER? ACCESS_MODIFIER)); 
model               :       MODEL;
model_name          :       VARIABLE_NAME;
parents             :       INHERITANCE_TYPE (VARIABLE_NAME ','?)+;
class_header        :       annotation* modifier model model_name parents?;
variable_type       :       (VARIABLE_TYPE | VARIABLE_NAME);
variable            :       (VARIABLE_NAME | KEY_VALUE);
class_attribute     :       modifier variable_type variable;
class_              :       class_header '{' class_attribute* function_* '}';

return_type         :       (VARIABLE_TYPE | VARIABLE_NAME);
method_name         :       VARIABLE_NAME;
function_arg        :       (annotation? return_type variable ','?);
function_args       :       '(' function_arg* ')' | ANNOTATION_ARGS;
function_header     :       annotation? modifier return_type method_name? function_args*;
function_           :       function_header '{' function_body '}';
function_body       :       ~('}')*;

root                :       (import_
                    |       package
                    |       class_
                    )+      EOF;

