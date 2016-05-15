(define (flatten x)
  (cond ((null? x) '())
        ((pair? x) (append (flatten (car x)) (flatten (cdr x))))
        (else (list x))))

(define (interleave l1 l2)
      (if (null? l1) l2
          (if (null? l2) l1
              (cons (cons (car l1) (cadr l1)) (cons (cons (car l2) (cadr l2)) (interleave (cddr l1) (cddr l2)))))))

(define (line x y x2 y2)
  (let ((xOrg x) (yOrg y))
  (letrec ((lineCoor (lambda(x y x2 y2 a)
     (if (and (not(= y y2)) (= x x2))
       (if ( < y y2)
          (lineCoor x (+ y 1) x2 y2 (cons x (cons (+ y 1) a )))
          (if ( > y y2)
              (lineCoor x (- y 1) x2 y2 (cons x (cons (- y 1) a)))))
  (if ( = x x2)
      (append (append (cons xOrg '()) (cons yOrg '())) a)
     (if (> x x2)
             (lineCoor (+ x2 1) (+ y2 (/ (- y y2) (- x x2))) x y
                       (append a (cons (+ x2 1) (cons (+ y2 (/ (- y y2) (- x x2))) '()) )))
              (lineCoor (+ x 1) (+ y (/ (- y2 y) (- x2 x))) x2 y2 
                        (append a (cons (+ x 1)  (cons (+ y (/ (- y2 y) (- x2 x))) '()))))))))))
    (lineCoor x y x2 y2 '() ))))
 
(define (rectangle x1 y1 x2 y2)
  (if (= x1 x2)
  '()
  (flatten
   (cons
    (interleave
     (line x1 y1 x1 y2)
     (line x2 y1 x2 y2))
    (interleave
     (line x1 y1 x2 y1 )
   (line x1 y2 x2 y2)))
  )))


(define (circle centerX centerY r)
  (let((x 0) (y r) (d (/ (- 5 (* r 4)) 4)))
      (letrec ((GetCircleCoor (lambda (x y r d)
          (if (<= x y)
               (if (< d 0)    
                  (cons (+ centerX x) (cons (+ centerY y)                                                
                  (cons (- centerX x) (cons (+ centerY y)
                  (cons (+ centerX x) (cons (- centerY y)         
                  (cons (- centerX x) (cons (- centerY y)
                  (cons (+ centerX y) (cons (+ centerY x)
                  (cons (- centerX y) (cons (+ centerY x)
                  (cons (+ centerX y) (cons (- centerY x)
                  (cons (- centerX y) (cons (- centerY x)  
                               (GetCircleCoor (+ x 1) y r (+ d (+ (* x 2) 1)))))))))))))))))))
                  (cons (+ centerX x) (cons (+ centerY y)                                                
                  (cons (- centerX x) (cons (+ centerY y)
                  (cons (+ centerX x) (cons (- centerY y)         
                  (cons (- centerX x) (cons (- centerY y)
                  (cons (+ centerX y) (cons (+ centerY x)
                  (cons (- centerX y) (cons (+ centerY x)
                  (cons (+ centerX y) (cons (- centerY x)
                  (cons (- centerX y) (cons (- centerY x)  
                               (GetCircleCoor (+ x 1) (- y 1) r (+ d (+ (* 2 (- x y)) 1))))))))))))))))))))
               '() ))))
    (GetCircleCoor x y r d))))

(define (fill c g)
    (letrec ((fillCoor (lambda(g x)
        (if (or (or (null? g) (null? (cdr g)) (null? (cddr g))) (null? (cdddr g)))
          (flatten (append (append x g) c))
          (fillCoor (cddddr g)
          (cons (line (car g)
                               (cadr g)
                               (car (cddr g))
                               (car (cdddr g))) x))
          ))))
      (fillCoor g '())))


(define (removeEverySecondCoordinate l )
      (if (or (null? l) (null? (cdr l)) (null? (cddr l)) (null? (cdddr l)))
          l 
              (flatten (cons (cons (car l) (cadr l)) (removeEverySecondCoordinate (cddddr l))))))


(define (bounding-box x1 y1 x2 y2)
  (if (= x1 x2)
  '()
  (flatten
   (cons
    (interleave
     (removeEverySecondCoordinate (line x1 y1 x1 y2))
     (removeEverySecondCoordinate (line x2 y1 x2 y2)))
    (interleave
     (removeEverySecondCoordinate (line x1 y1 x2 y1 ))
     (removeEverySecondCoordinate (line x1 y2 x2 y2)))
  ))))


(define (filter x1 y1 x2 y2 l)
  (letrec ((filterCoor (lambda (x1 y1 x2 y2 l a)
  (if (or (null? l) (not (real? (car l)))) 
      (flatten (append a l)) 
      (if (and (and (and (>= (car l) x1) (<= (car l) x2) (>= (cadr l) y1))) (<= (cadr l) y2))
           (filterCoor x1 y1 x2 y2 (cddr l) (cons (cons (car l) (cadr l)) a))
           (filterCoor x1 y1 x2 y2 (cddr l) a )))))) 
    (filterCoor x1 y1 x2 y2 l '())))

(define (text-at x1 y1 t)
  (cons  x1 (cons y1 (cons t '()))))