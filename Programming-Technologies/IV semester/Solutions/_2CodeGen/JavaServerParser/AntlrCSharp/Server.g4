grammar Server;

PACKAGE                     :       'package';
IMPORT                      :       'import';
MODEL_TYPE                       :       'class' | 'interface';
ACCESS_MODIFIER             :       'public' | 'private' | 'protected';
NON_ACCESS_MODIFIER         :       'static' | 'abstract' | 'final';
INHERITANCE_TYPE            :       'extends' | 'implements';

WS                  :       [ ;\n\t] -> skip;

fragment LETTER     :       [A-Za-z];  
fragment WORD       :       LETTER+;
fragment DIGIT      :       [0-9];
fragment NUMBER     :       DIGIT+ ('.,' DIGIT+)?;

NAME       :       (LETTER | '_') (LETTER | DIGIT | '_')*;
TYPE       :       (LETTER | '<' | '>' | DIGIT | '_')+;
PATH        :       ((WORD '.'?)* '*') | ((WORD '.'?)+ '*'?);

KEY_VALUE           :       NAME ' '? '=' ' '? VALUE;
fragment VALUE      :      ((STRING | TYPE | ANNOTATION_ARGS) ' '?)+;
fragment STRING              :       '"' ANY+ '"';
ANY                 :       ~([\n;])+?;

package_name        :       PACKAGE;
import_name         :       IMPORT;
path                :       (PATH | NAME);

package             :       package_name path;
import_             :       import_name path;

ANNOTATION_HEADER   :       '@' NAME;
annotation_header   :       ANNOTATION_HEADER;
arguments           :       ANNOTATION_ARGS;
annotation          :       annotation_header arguments?;
fragment ANNOTATION_ARG      :       (KEY_VALUE | VALUE);
ANNOTATION_ARGS     :       '(' (ANNOTATION_ARG ','? ' '?)* ')';

modifier            :       ((ACCESS_MODIFIER NON_ACCESS_MODIFIER?)
                            | (NON_ACCESS_MODIFIER? ACCESS_MODIFIER)); 
model_type               :       MODEL_TYPE;
model_name          :       NAME;
parents             :       INHERITANCE_TYPE (NAME ','?)+;
class_header        :       annotation* modifier model_type model_name parents?;
variable_type       :       (TYPE | NAME);
variable            :       (NAME | KEY_VALUE);
class_attribute     :       modifier variable_type variable;
class_              :       class_header '{' class_attribute* function_* '}';

return_type         :       (TYPE | NAME);
method_name         :       NAME;
function_arg        :       (annotation? return_type variable ','?);
function_args       :       '(' function_arg* ')' | ANNOTATION_ARGS;
function_header     :       annotation? modifier return_type method_name? function_args*;
function_           :       function_header '{' function_body '}';
function_body       :       ~('}')*;

root                :       (import_
                    |       package
                    |       class_
                    )+      EOF;