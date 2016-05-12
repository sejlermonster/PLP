(define (line x y x2 y2)
  (let ((xOrg x) (yOrg y))
  (letrec ((lineCoor (lambda(x y x2 y2 a)
     (if (and (not(= y y2)) (= x x2))
       (if ( < y y2)
          (lineCoor x (+ y 1) x2 y2 (cons x (cons (+ y 1) a )))
          (if ( > y y2)
              (lineCoor x (- y 1) x2 y2 (cons x (cons (- y 1) a)))))
  (if ( = x x2)
      (append (append a (cons xOrg '())) (cons yOrg '()))
     (if (> x x2)
             (lineCoor (+ x2 1) (+ y2 (/ (- y y2) (- x x2))) x y
                       (cons (+ x2 1) (cons  (+ y2 (/ (- y y2) (- x x2))) a)))
              (lineCoor (+ x 1) (+ y (/ (- y2 y) (- x2 x))) x2 y2 
                        (cons (+ x 1) (cons  (+ y (/ (- y2 y) (- x2 x))) a)))))))))
    (lineCoor x y x2 y2 '() ))))


(define (flatten x)
  (cond ((null? x) '())
        ((pair? x) (append (flatten (car x)) (flatten (cdr x))))
        (else (list x))))

(define (rectangleOld x1 y1 x2 y2)
  (if (= x1 x2)
  '()
  (flatten
  (cons
   (cons
    (cons
     (line x1 y1 x1 y2)
     (line x1 y2 x2 y2))
    (line x1 y1 x2 y1))
	(line x2 y2 x2 y1 )))
  ))

(define (merge l1 l2)
      (if (null? l1) l2
          (if (null? l2) l1
              (cons (cons (car l1) (cadr l1)) (cons (cons (car l2) (cadr l2)) (merge (cdr (cdr l1)) (cdr (cdr l2))))))))

(define (rectangle x1 y1 x2 y2)
  (if (= x1 x2)
  '()
  (flatten
   (cons
    (merge
     (line x1 y1 x1 y2)
     (line x2 y1 x2 y2))
    (merge
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
      (if (null? g)
          (flatten x)
          (fillCoor (cdr (cdr (cdr (cdr g))))
          (cons (line (car g)
                               (cadr g)
                               (car (cdr (cdr g)))
                               (car (cdr (cdr (cdr g))))) x))
          ))))
      (fillCoor g '())))

(define (fill2 c g)
    (letrec ((fillCoor (lambda(g x)
      (if (null? g)
          (flatten x)
          (fillCoor (cdr (cdr (reverse (cdr (cdr (reverse g))))))
                   (cons (line (car g)
                               (cadr g)
                               (cadr (reverse g))
                               (car (reverse g))) x))
          ))))
      (fillCoor g '())))

(define (fill3 c g)
  (let ((pointX (car g)) (pointY (cadr g)))
     (letrec ((fillCoor (lambda(g x)
      (if (null? g) 
          (flatten x)
          (fillCoor (cdr (cdr g))
          (cons (line pointX pointY
                         (car g)
                         (cadr g)) x))
          ))))
      (fillCoor g '()))))