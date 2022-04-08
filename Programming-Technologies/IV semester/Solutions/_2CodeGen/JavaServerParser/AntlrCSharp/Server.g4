grammar Server;

PACKAGE             :       'package';
IMPORT              :       'import';
MODEL               :       ('class' | 'interface');
ACCESS_MODIFIER     :       ('public' | 'private' | 'protected');
NON_ACCESS_MODIFIER :       ('static' | 'abstract' | 'final');
INHERITANCE_TYPE    :       ('extends' | 'implements');

WS                  :       [ ;\n\t] -> skip;

fragment LETTER     :       [A-Za-z];  
fragment WORD       :       LETTER+;
fragment DIGIT      :       [0-9];
fragment NUMBER     :       DIGIT+ ('.,' DIGIT+)?;

VARIABLE_NAME       :       (LETTER | '_') (LETTER | DIGIT | '_')*;
VARIABLE_TYPE       :       (LETTER | '<' | '>' | DIGIT | '_')+;
INCLUDE_PATH        :       ((WORD '.'?)* '*') | ((WORD '.'?)+ '*'?);

ANNOTATION_HEADER   :       '@' WORD;
ANNOTATION_ARGS     :       '(' ((KEY_VALUE | VALUE)','? ' '?)* ')';
fragment KEY        :       VARIABLE_NAME;
fragment VALUE      :       (LETTER | '_' | '/' | '"' | '{' | '}' | DIGIT)+;
KEY_VALUE           :        KEY ' '? '=' ' '? VALUE;

import_             :       IMPORT INCLUDE_PATH;
package             :       PACKAGE INCLUDE_PATH;
annotation          :       ANNOTATION_HEADER ANNOTATION_ARGS?;

modifier            :       ((ACCESS_MODIFIER NON_ACCESS_MODIFIER?) | (NON_ACCESS_MODIFIER? ACCESS_MODIFIER)); 
model               :       MODEL VARIABLE_NAME;
parents             :       INHERITANCE_TYPE (VARIABLE_NAME ','?)+;
class_header        :       annotation* modifier model parents?;
variable_type       :       (VARIABLE_TYPE | VARIABLE_NAME);
variable            :       (KEY_VALUE | VARIABLE_NAME);
class_attribute     :       modifier variable_type variable;
class_              :       class_header '{' class_attribute* function_* '}';

return_type         :       variable_type;
method_name         :       VARIABLE_NAME;
method_argument     :       annotation? variable_type variable;
function_header     :       annotation? modifier return_type method_name '(' method_argument* ')';
function_           :       function_header;

root                :       (import_
                    |       package
                    |       class_
                    )+      EOF;
