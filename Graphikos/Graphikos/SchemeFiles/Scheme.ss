(define (line x y x2 y2)
  (if (and (not(= y y2)) (= x x2))
       (if ( < y y2)
          (cons x (cons (+ y 1) (line x (+ y 1) x2 y2)))
          (if ( > y y2)
              (cons x (cons (- y 1) (line x (- y 1) x2 y2)))))
  (if ( = x x2)
       '()
       (cons (+ x 1) (cons  (+ y (/ (- y2 y) (- x2 x)))
              (line (+ x 1) (+ y (/ (- y2 y) (- x2 x))) x2 y2 ))
        ))))


(define (flatten x)
  (cond ((null? x) '())
        ((pair? x) (append (flatten (car x)) (flatten (cdr x))))
        (else (list x))))


(define (rectangle x1 y1 x2 y2)
  (if (= x1 x2)
  '()
  (flatten
  (cons
   (cons
    (cons
     (line x1 y1 x1 y2)
     (line x1 y2 x2 y2))
    (line x2 y2 x2 y1 ))
   (line x1 y1 x2 y1)))
  ))